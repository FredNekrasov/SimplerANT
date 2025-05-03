package com.fredprojects.antandroidapp.presentation.core.components

import androidx.compose.foundation.*
import androidx.compose.foundation.layout.*
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Close
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.fredprojects.antandroidapp.data.ArticleDto
import com.fredprojects.antandroidapp.presentation.core.*

/**
 * A composable function that displays the details of an [ArticleDto] in a scrollable column.
 * The details include: type, title, date, image, and description.
 * It also includes a close button at the bottom of the dialog.
 * Clicking on the close button will call the [isShowDialog] function with a parameter of `false`.
 *
 * @param isShowDialog a function that is called when the close button is clicked
 * @param article the article to be shown in the details
 */
@Composable
inline fun ListItemDialog(
    crossinline isShowDialog: (Boolean) -> Unit,
    article: ArticleDto,
    modifier: Modifier = Modifier
) {
    Column(modifier.verticalScroll(rememberScrollState()).background(MaterialTheme.colorScheme.background)) {
        ANTTitle(article.catalog.name)
        Spacer(Modifier.height(4.dp))
        ANTTitle(article.title)
        if(article.dateOrBanner.isNotBlank()) ANTText(article.dateOrBanner, modifier = Modifier.padding(end = 4.dp).align(Alignment.End))
        Spacer(Modifier.height(4.dp))
        if(article.content.isNotEmpty()) ImageSlider(article = article, Modifier.aspectRatio(1.5f))
        Spacer(Modifier.height(4.dp))
        ANTText(article.description, modifier = Modifier.padding(8.dp))
        ANTIconButton(
            onClick = { isShowDialog(false) }, icon = Icons.Default.Close,
            modifier = Modifier.align(Alignment.CenterHorizontally)
                .border(2.dp, MaterialTheme.colorScheme.error, MaterialTheme.shapes.medium)
        )
    }
}