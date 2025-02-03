package com.fredprojects.antandroidapp

import android.app.Application
import com.fredprojects.antandroidapp.di.appModule
import org.koin.android.ext.koin.androidContext
import org.koin.android.ext.koin.androidLogger
import org.koin.core.context.startKoin
import org.koin.core.context.stopKoin

class ANTApp : Application() {
    override fun onCreate() {
        super.onCreate()
        startKoin {
            androidContext(this@ANTApp)
            androidLogger()
            modules(appModule)
        }
    }
    override fun onTerminate() {
        super.onTerminate()
        stopKoin()
    }
}