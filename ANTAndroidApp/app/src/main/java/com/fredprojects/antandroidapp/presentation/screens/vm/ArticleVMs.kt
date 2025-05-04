package com.fredprojects.antandroidapp.presentation.screens.vm

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.fredprojects.antandroidapp.data.ArticleRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.flow.update
import kotlinx.coroutines.launch

class MainArticleVM(private val repository: ArticleRepository) : ViewModel() {
    private val articlesMSF = MutableStateFlow(MainArticleState())
    val articlesSF = articlesMSF.asStateFlow()
    private fun getArticles(catalogId: Long, pageNumber: Long = 1L) {
        viewModelScope.launch {
            articlesMSF.update { it.copy(isLoading = true) }
            repository.getList(catalogId, pageNumber).onSuccess { list ->
                articlesMSF.update { it.copy(list = list, isLoading = false) }
            }.onFailure {
                articlesMSF.update { it.copy(isLoading = false, hasError = true) }
            }
        }
    }
    init { getArticles(1) }
}

abstract class ArticleVM(private val repository: ArticleRepository) : ViewModel() {
    private val articlesMSF = MutableStateFlow(PagedArticleState())
    val articlesSF = articlesMSF.asStateFlow()
    fun getArticles(catalogId: Long, pageNumber: Int = 1) {
        viewModelScope.launch {
            articlesMSF.update { it.copy(isLoading = true) }
            repository.getList(catalogId, pageNumber.toLong()).onSuccess { list ->
                articlesMSF.update {
                    if(list.size < 50) it.copy(map = mapOf(pageNumber to list), isLoading = false)
                    else it.copy(map = mapOf(pageNumber to list), isLoading = false, hasNextData = true)
                }
            }.onFailure {
                articlesMSF.update { it.copy(isLoading = false, hasError = true) }
            }
        }
    }
}

class ParishLifeVM(repository: ArticleRepository) : ArticleVM(repository) {
    fun getParishLifeArticles(pageNumber: Int = 1) = super.getArticles(2, pageNumber)
    init { getParishLifeArticles() }
}

class YouthClubVM(repository: ArticleRepository) : ArticleVM(repository) {
    fun getYouthClubArticles(pageNumber: Int = 1) = super.getArticles(5, pageNumber)
    init { getYouthClubArticles() }
}

class AdvicesVM(repository: ArticleRepository) : ArticleVM(repository) {
    fun getAdviceArticles(pageNumber: Int = 1) = super.getArticles(7, pageNumber)
    init { getAdviceArticles() }
}

class HistoryVM(repository: ArticleRepository) : ArticleVM(repository) {
    fun getHistoryArticles(pageNumber: Int = 1) = super.getArticles(8, pageNumber)
    init { getHistoryArticles() }
}

class StoriesVM(repository: ArticleRepository) : ArticleVM(repository) {
    fun getStoryArticles(pageNumber: Int = 1) = super.getArticles(13, pageNumber)
    init { getStoryArticles() }
}