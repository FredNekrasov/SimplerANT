package com.fredprojects.antandroidapp.data.remote.dto

import kotlinx.serialization.Serializable

@Serializable
data class ArticleResponse(
    val pageNumber: Long,
    val pageSize: Long,
    val totalRecords: Long,
    val data: List<ArticleDto>
)