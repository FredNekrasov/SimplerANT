package com.fredprojects.antandroidapp.presentation.screens.onePages

import androidx.compose.foundation.layout.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.MailOutline
import androidx.compose.material.icons.filled.Phone
import androidx.compose.material3.HorizontalDivider
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalUriHandler
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.util.fastForEach
import com.fredprojects.antandroidapp.presentation.core.*
import com.fredprojects.antandroidapp.presentation.screens.vm.MainArticleState

@Composable
fun Sacraments(
    state: MainArticleState,
    modifier: Modifier = Modifier
) {
    val uriHandler = LocalUriHandler.current
    Column(modifier.padding(8.dp), Arrangement.Center) {
        state.list.fastForEach {
            if(it.catalog.name != ANTStrings.SACRAMENTS) return@fastForEach
            ANTTitle(text = it.title)
            Spacer(modifier = Modifier.height(8.dp))
            ANTText(text = it.description, textAlign = TextAlign.Center)
        }
        Spacer(modifier = Modifier.height(8.dp))
        HorizontalDivider()
        Spacer(modifier = Modifier.height(8.dp))
        state.list.fastForEach { it ->
            if(it.catalog.name != ANTStrings.CONTACTS) return@fastForEach
            ANTTitle(it.title)
            Spacer(modifier = Modifier.height(8.dp))
            ContactsCard(contentList = it.content) { uriHandler.openUri(it) }
        }
    }
}
/**
 * Contacts card with buttons for phone, telegram, vk, email
 * @param contentList list of the contact information
 * @param openSomeApp action for opening app like telegram, vk, gmail and phone
 */
@Composable
private inline fun ContactsCard(contentList: List<String>, crossinline openSomeApp: (String) -> Unit) {
    Row(Modifier.fillMaxWidth(), Arrangement.SpaceEvenly, Alignment.CenterVertically) {
        ANTIconButton({ openSomeApp(contentList.getNotNull(2)) }, Icons.Default.Phone)
        ANTTextButton({ openSomeApp(contentList.getNotNull(0)) }, ANTStrings.TELEGRAM)
        ANTTextButton({ openSomeApp(contentList.getNotNull(1)) }, ANTStrings.VK)
        ANTIconButton({ openSomeApp(contentList.getNotNull(3)) }, Icons.Default.MailOutline)
    }
}
private fun List<String>.getNotNull(index: Int): String = getOrNull(index)?.toString() ?: ""