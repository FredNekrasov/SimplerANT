package com.fredprojects.antandroidapp.data.remote.dto

import kotlinx.serialization.Serializable

@Serializable
data class ArticleDto(
    val id: Long,
    val catalog: CatalogDto,
    val title: String,
    val description: String,
    val dateOrBanner: String,
    val content: List<String>
)