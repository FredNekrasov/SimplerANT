package com.fredprojects.antandroidapp.data.remote.dto

import kotlinx.serialization.Serializable

@Serializable
data class CatalogDto(
    val id: Long,
    val name: String
)