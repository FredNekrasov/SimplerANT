package com.fredprojects.antandroidapp.data

import io.ktor.client.HttpClient
import io.ktor.client.call.body
import io.ktor.client.request.get
import java.time.LocalDate

/**
 * @param articleDao is an ArticleDao class. It is used to get data from the database and add data to the database
 * @param articleStatusDao is an ArticleStatusDao class. It is used to manage article status data from the database
 * @param client is a HttpClient. It is used to get data from the server
 */
class ArticleRepository(
    private val articleDao: ArticleDao,
    private val articleStatusDao: ArticleStatusDao,
    private val client: HttpClient
) {
    /**
     * Get list of articles by catalog id and page number from the database and return it using a [Result.success] if there is no internet connection.
     * If there is internet connection, get list of articles by catalog id and page number from the server and return it as a [Result]
     *
     * @param catalogId The ID of the catalog to retrieve the list of articles from.
     * @param pageNumber The number of the page to retrieve the list of articles from.
     *
     * @return A [Result] object containing the list of articles if the request was successful, or a [NullPointerException] if the request failed.
     */
    suspend fun getList(catalogId: Long, pageNumber: Long) : Result<List<ArticleDto>> = runCatching {
        val articleList = articleDao.getListBy(catalogId, pageNumber).map { it.toDto() }
        if(!hasNewArticles(catalogId, pageNumber)) return Result.success(articleList)
        val response = client.get("http://ip:port/api/chapters/$catalogId?pageNumber=$pageNumber").body<ArticleResponse?>()
        return when {
            response == null -> Result.failure(NullPointerException())
            articleDao.getCountAll() == response.totalRecords -> Result.success(articleList)
            else -> {
                refreshData(response, catalogId)
                Result.success(response.data)
            }
        }
    }
    /**
     * refreshData function is used to refresh the data in the database
     * @param response is an ArticleResponse class that is received from the server
     * @param catalogId is an ID of the catalog
     * @see ArticleResponse
     */
    private suspend inline fun refreshData(response: ArticleResponse, catalogId: Long) {
        articleDao.deleteBy(catalogId, response.pageNumber)
        response.data.forEach { articleDao.upsert(it.toEntity(catalogId, response.pageNumber)) }
        val articleStatus = articleStatusDao.getBy(catalogId, response.pageNumber) ?: ArticleStatusEntity(
            id = articleStatusDao.getCountAll(),
            catalogId = catalogId,
            pageNumber = response.pageNumber,
            currentDate = LocalDate.now().toString()
        )
        articleStatusDao.upsert(articleStatus.copy(currentDate = LocalDate.now().toString()))
    }
    /** hasNewArticles function is used to check if there are new articles in the server **/
    private suspend fun hasNewArticles(catalogId: Long, pageNumber: Long): Boolean {
        val lastArticleStatus = articleStatusDao.getBy(catalogId, pageNumber) ?: return true
        return !lastArticleStatus.currentDate.contentEquals(LocalDate.now().toString())
    }
}