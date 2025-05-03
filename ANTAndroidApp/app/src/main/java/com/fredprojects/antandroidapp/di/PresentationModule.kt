package com.fredprojects.antandroidapp.di

import com.fredprojects.antandroidapp.data.ArticleRepository
import com.fredprojects.antandroidapp.presentation.screens.vm.*
import org.koin.core.module.dsl.viewModel
import org.koin.core.qualifier.qualifier
import org.koin.dsl.module

val presentationModule = module {
    viewModel(qualifier<MainArticleVM>()) {
        MainArticleVM(get(qualifier<ArticleRepository>()))
    }
    viewModel(qualifier<ParishLifeVM>()) {
        ParishLifeVM(get(qualifier<ArticleRepository>()))
    }
    viewModel(qualifier<YouthClubVM>()) {
        YouthClubVM(get(qualifier<ArticleRepository>()))
    }
    viewModel(qualifier<AdvicesVM>()) {
        AdvicesVM(get(qualifier<ArticleRepository>()))
    }
    viewModel(qualifier<HistoryVM>()) {
        HistoryVM(get(qualifier<ArticleRepository>()))
    }
    viewModel(qualifier<StoriesVM>()) {
        StoriesVM(get(qualifier<ArticleRepository>()))
    }
}