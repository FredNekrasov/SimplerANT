package com.fredprojects.antandroidapp.data.local

import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

/**
 * ArticleDao is used for working with the database
 * @param db is an ANTDatabase class
 */
class ArticleDao(
    private val db: ANTDatabase
) {
    /** Get count of all articles from the database **/
    suspend fun getCountAllArticles(): Long = withContext(Dispatchers.IO) {
        db.articleDaoQueries.getCountAllArticles().executeAsOne()
    }
    /**
     * Get articles filtered by catalog id and page number from the database
     * @param catalogId is an ID of the catalog
     * @param pageNumber is a number of the page
     * @return List of articles
     */
    suspend fun getArticlesBy(catalogId: Long, pageNumber: Long): List<ArticleEntity> = withContext(Dispatchers.IO) {
        db.articleDaoQueries.getArticlesBy(catalogId, pageNumber).executeAsList()
    }
    /**
     * Upsert article in the database
     * @param article is an Article class that will be inserted in the database if it doesn't exist or updated otherwise
     */
    suspend fun upsertArticle(article: ArticleEntity) = withContext(Dispatchers.IO) {
        db.articleDaoQueries.upsertArticle(article)
    }
    suspend fun deleteArticle(id: Long) = withContext(Dispatchers.IO) {
        db.articleDaoQueries.deleteArticle(id)
    }
}