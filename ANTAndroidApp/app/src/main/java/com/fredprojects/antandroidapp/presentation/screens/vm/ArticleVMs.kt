package com.fredprojects.antandroidapp.presentation.screens.vm

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.fredprojects.antandroidapp.data.repository.ArticleRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.asStateFlow
import kotlinx.coroutines.launch

class MainArticleVM(
    private val repository: ArticleRepository
) : ViewModel() {
    private val articlesMSF = MutableStateFlow(MainArticleState())
    val articlesSF = articlesMSF.asStateFlow()
    private fun getArticles(catalogId: Long, pageNumber: Long = 1L) {
        viewModelScope.launch {
            articlesMSF.emit(articlesSF.value.copy(isLoading = true))
            repository.getList(catalogId, pageNumber).onSuccess {
                val newState = articlesSF.value.copy(list = it, isLoading = false)
                articlesMSF.emit(newState)
            }.onFailure {
                articlesMSF.emit(articlesSF.value.copy(isLoading = false, isError = true))
            }
        }
    }
    init { getArticles(1) }
}

open class OpenArticleVM(
    private val repository: ArticleRepository
) : ViewModel() {
    private val articlesMSF = MutableStateFlow(PagedArticleState())
    val articlesSF = articlesMSF.asStateFlow()
    open fun getArticles(catalogId: Long, pageNumber: Int = 1) {
        viewModelScope.launch {
            articlesMSF.emit(articlesSF.value.copy(isLoading = true))
            repository.getList(catalogId, pageNumber.toLong()).onSuccess {
                val newState = if(it.size < 50)  articlesSF.value.copy(map = mapOf(pageNumber to it), isLoading = false)
                else articlesSF.value.copy(map = mapOf(pageNumber to it), isLoading = false, hasNextData = true)
                articlesMSF.emit(newState)
            }.onFailure {
                articlesMSF.emit(articlesSF.value.copy(isLoading = false, isError = true))
            }
        }
    }
}

class ParishLifeVM(repository: ArticleRepository) : OpenArticleVM(repository) {
    fun getParishLifeArticles(pageNumber: Int = 1) {
        super.getArticles(2, pageNumber)
    }
    init { getArticles(2) }
}

class YouthClubVM(repository: ArticleRepository) : OpenArticleVM(repository) {
    fun getYouthClubArticles(pageNumber: Int = 1) {
        super.getArticles(5, pageNumber)
    }
    init { getArticles(5) }
}

class AdvicesVM(repository: ArticleRepository) : OpenArticleVM(repository) {
    fun getAdviceArticles(pageNumber: Int = 1) {
        super.getArticles(7, pageNumber)
    }
    init { getArticles(7) }
}

class HistoryVM(repository: ArticleRepository) : OpenArticleVM(repository) {
    fun getHistoryArticles(pageNumber: Int = 1) {
        super.getArticles(8, pageNumber)
    }
    init { getArticles(8) }
}

class StoriesVM(repository: ArticleRepository) : OpenArticleVM(repository) {
    fun getStoryArticles(pageNumber: Int = 1) {
        super.getArticles(13, pageNumber)
    }
    init { getArticles(13) }
}