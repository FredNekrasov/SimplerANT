package com.fredprojects.antandroidapp.data.local

import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext

/**
 * ArticleStatusDao is used for working with the database
 * @param db is an ANTDatabase class
 */
class ArticleStatusDao(
    private val db: ANTDatabase
) {
    /**
     * Get filtered list of article status from the database
     * @return List of articles
     */
    suspend fun getArticleStatusBy(catalogId: Long, pageNumber: Long): ArticleStatusEntity? = withContext(Dispatchers.IO) {
        db.articleStatusDaoQueries.getArticleStatusBy(catalogId, pageNumber).executeAsOneOrNull()
    }
    /**
     * Get count of all article status from the database
     * @return Long
     */
    suspend fun getCountAllArticleStatus(): Long = withContext(Dispatchers.IO) {
        db.articleStatusDaoQueries.getCountAllArticleStatus().executeAsOne()
    }
    /**
     * Upsert article status in the database
     * @param articleStatus is an ArticleStatus class that will be inserted in the database if it doesn't exist or updated otherwise
     */
    suspend fun upsertArticleStatus(articleStatus: ArticleStatusEntity) = withContext(Dispatchers.IO) {
        db.articleStatusDaoQueries.upsertArticleStatus(articleStatus)
    }
    /**
     * Delete article status from the database
     * @param id is an ID of the article that will be deleted
     */
    suspend fun deleteArticleStatus(id: Long) = withContext(Dispatchers.IO) {
        db.articleStatusDaoQueries.deleteArticleStatus(id)
    }
}