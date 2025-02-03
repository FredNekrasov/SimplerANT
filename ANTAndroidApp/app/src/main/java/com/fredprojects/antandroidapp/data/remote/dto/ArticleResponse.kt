package com.fredprojects.antandroidapp.data.remote.dto

import kotlinx.serialization.Serializable

/**
 * ArticleResponse would be received from the server
 * @param pageNumber is a number of the page
 * @param pageSize is a size of the page
 * @param totalRecords is a total number of records
 * @param data is a list of articles
 */
@Serializable
data class ArticleResponse(
    val pageNumber: Long,
    val pageSize: Long,
    val totalRecords: Long,
    val data: List<ArticleDto>
)