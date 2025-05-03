package com.fredprojects.antandroidapp.di

import org.koin.dsl.module

val appModule = module {
    includes(dataModule, presentationModule)
}