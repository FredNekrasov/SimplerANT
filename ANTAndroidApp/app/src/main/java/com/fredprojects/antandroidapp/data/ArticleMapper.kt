package com.fredprojects.antandroidapp.data

import kotlinx.serialization.json.Json

/**  Mapper for converting [ArticleDto] to [ArticleEntity] */
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
/**  Mapper for converting [ArticleEntity] to [ArticleDto] */
fun ArticleEntity.toDto() = ArticleDto(
    id = id,
    title = title,
    description = description,
    dateOrBanner = date,
    catalog = CatalogDto(catalogId, articleType),
    content = Json.decodeFromString(content)
)