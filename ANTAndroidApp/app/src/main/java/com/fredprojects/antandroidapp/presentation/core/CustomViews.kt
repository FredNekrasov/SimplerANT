package com.fredprojects.antandroidapp.presentation.core

import androidx.compose.foundation.border
import androidx.compose.foundation.layout.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.outlined.Menu
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.text.font.FontFamily
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.core.net.toUri
import coil.compose.AsyncImage

typealias Action = () -> Unit
@Composable
fun ANTText(text: String, modifier: Modifier = Modifier, textAlign: TextAlign = TextAlign.Justify) {
    Text(
        text,
        modifier,
        fontFamily = FontFamily.Serif,
        textAlign = textAlign
    )
}
@Composable
fun ANTTitle(text: String) {
    Text(
        text,
        Modifier.fillMaxWidth(),
        fontWeight = FontWeight.SemiBold,
        fontFamily = FontFamily.Serif,
        textAlign = TextAlign.Center
    )
}
@Composable
fun ANTTextButton(onClick: Action, text: String, modifier: Modifier = Modifier) {
    TextButton(onClick, modifier) {
        ANTText(text)
    }
}
@Composable
fun ANTIconButton(onClick: Action, icon: ImageVector, modifier: Modifier = Modifier, enabled: Boolean = true) {
    IconButton(onClick, modifier, enabled) {
        Icon(icon, icon.toString())
    }
}
@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun ANTTopAppBar(openDrawer: Action) {
    TopAppBar(
        title = { ANTText(ANTStrings.MAIN_TITLE) },
        Modifier.fillMaxWidth(),
        navigationIcon = { ANTIconButton(openDrawer, Icons.Outlined.Menu) }
    )
}
@Composable
fun ANTNavigationDrawerItem(text: String, selected: Boolean, onClick: Action) {
    NavigationDrawerItem(
        label = { ANTText(text) },
        selected,
        onClick,
        Modifier.fillMaxWidth(),
        shape = MaterialTheme.shapes.small,
        colors = NavigationDrawerItemDefaults.colors()
    )
}
@Composable
fun ANTFloatingActionButton(onClick: Action, icon: ImageVector) {
    FloatingActionButton(
        onClick = onClick,
        modifier = Modifier.border(1.dp, MaterialTheme.colorScheme.primary, MaterialTheme.shapes.medium),
        shape = MaterialTheme.shapes.medium
    ) {
        Icon(icon, ANTStrings.SCHEDULE)
    }
}
@Composable
fun ANTCard(onClick: Action, uri: String?, title: String, date: String, modifier: Modifier = Modifier) {
    Card(
        onClick,
        modifier.border(DividerDefaults.Thickness, MaterialTheme.colorScheme.primary, MaterialTheme.shapes.small).padding(4.dp),
        shape = MaterialTheme.shapes.small,
        colors = CardDefaults.outlinedCardColors()
    ) {
        if(!uri.isNullOrBlank()) Box(Modifier.fillMaxHeight(0.2f).fillMaxWidth()) {
            AsyncImage(model = uri.toUri(), contentDescription = title)
        }
        Text(title, Modifier.align(Alignment.CenterHorizontally), fontFamily = FontFamily.Serif, textAlign = TextAlign.Center, overflow = TextOverflow.Ellipsis, maxLines = 1)
        ANTText(date, Modifier.align(Alignment.End))
    }
}