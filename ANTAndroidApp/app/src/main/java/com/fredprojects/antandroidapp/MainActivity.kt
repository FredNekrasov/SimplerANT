package com.fredprojects.antandroidapp

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import com.fredprojects.antandroidapp.navigation.MainEntryPoint
import com.fredprojects.antandroidapp.ui.theme.ANTAndroidAppTheme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            ANTAndroidAppTheme {
                MainEntryPoint()
            }
        }
    }
}