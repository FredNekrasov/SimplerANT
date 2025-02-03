package com.fredprojects.antandroidapp.data.mapper

import com.fredprojects.antandroidapp.data.local.ArticleEntity
import com.fredprojects.antandroidapp.data.remote.dto.ArticleDto
import com.fredprojects.antandroidapp.data.remote.dto.CatalogDto
import kotlinx.serialization.json.Json

/**
 *  Mapper for mapping ArticleDto to ArticleEntity and vice versa
 *  @return ArticleEntity
 */
fun ArticleDto.toEntity(catalogId: Long, pageNumber: Long) = ArticleEntity(
    id = id,
    title = title,
    description = description,
    date = dateOrBanner,
    articleType = catalog.name,
    pageNumber = pageNumber,
    catalogId = catalogId,
    content = Json.encodeToString(content)
)

fun ArticleEntity.toDto() = ArticleDto(
    id = id,
    title = title,
    description = description,
    dateOrBanner = date,
    catalog = CatalogDto(catalogId, articleType),
    content = Json.decodeFromString(content)
)