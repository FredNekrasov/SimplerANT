package com.fredprojects.antandroidapp.presentation.screens.onePages

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.util.fastForEach
import com.fredprojects.ant.presentation.core.ANTStrings
import com.fredprojects.antandroidapp.presentation.core.*
import com.fredprojects.antandroidapp.presentation.core.components.ImageSlider
import com.fredprojects.antandroidapp.presentation.screens.vm.MainArticleState

@Composable
fun Priesthood(
    state: MainArticleState,
    modifier: Modifier = Modifier
) {
    Column(modifier.verticalScroll(rememberScrollState()).padding(8.dp)) {
        state.list.fastForEach {
            if(it.catalog.name != ANTStrings.PRIESTHOOD) return@fastForEach
            FredTitle(it.catalog.name)
            Spacer(Modifier.height(4.dp))
            ImageSlider(it, Modifier.fillMaxWidth().aspectRatio(0.58f))
            Spacer(Modifier.height(4.dp))
            FredTitle(it.title)
            Spacer(Modifier.height(4.dp))
            FredText(it.description)
        }
    }
}