package com.fredprojects.antandroidapp.presentation.screens.vm

import androidx.compose.runtime.Stable
import com.fredprojects.antandroidapp.data.ArticleDto

@Stable
data class MainArticleState(
    val list: List<ArticleDto> = emptyList(),
    val isLoading: Boolean = false,
    val hasError: Boolean = false
)

@Stable
data class PagedArticleState(
    val map: Map<Int, List<ArticleDto>> = emptyMap(),
    val isLoading: Boolean = false,
    val hasError: Boolean = false,
    val hasNextData: Boolean = false
)