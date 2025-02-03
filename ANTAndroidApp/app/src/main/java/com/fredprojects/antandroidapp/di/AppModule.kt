package com.fredprojects.antandroidapp.di

import org.koin.dsl.module

val appModule get() = module {
    includes(dataModule, presentationModule)
}