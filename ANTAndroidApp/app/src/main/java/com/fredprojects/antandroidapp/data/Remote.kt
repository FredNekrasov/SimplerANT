package com.fredprojects.antandroidapp.data

import kotlinx.serialization.Serializable

@Serializable
data class CatalogDto(
    val id: Long,
    val name: String
)
@Serializable
data class ArticleDto(
    val id: Long,
    val catalog: CatalogDto,
    val title: String,
    val description: String,
    val dateOrBanner: String,
    val content: List<String>
)
@Serializable
data class ArticleResponse(
    val pageNumber: Long,
    val pageSize: Long,
    val totalRecords: Long,
    val data: List<ArticleDto>
)