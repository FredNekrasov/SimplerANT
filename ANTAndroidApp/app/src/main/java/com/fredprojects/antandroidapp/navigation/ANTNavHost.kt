package com.fredprojects.antandroidapp.navigation

import android.content.Context
import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.MaterialTheme
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.snapshots.SnapshotStateList
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.navigation.NavHostController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import com.fredprojects.antandroidapp.presentation.core.ANTStrings
import com.fredprojects.antandroidapp.presentation.screens.*
import com.fredprojects.antandroidapp.presentation.screens.onePages.*
import com.fredprojects.antandroidapp.presentation.screens.vm.*
import org.koin.androidx.compose.koinViewModel
import org.koin.core.qualifier.qualifier

@Composable
fun ANTNavHost(
    controller: NavHostController,
    modifier: Modifier = Modifier,
    navItems: SnapshotStateList<String> = ANTStrings.screens
) {
    val mainVM: MainArticleVM = koinViewModel(qualifier<MainArticleVM>())
    val parishLifeVM: ParishLifeVM = koinViewModel(qualifier<ParishLifeVM>())
    val youthClubVM: YouthClubVM = koinViewModel(qualifier<YouthClubVM>())
    val advicesVM: AdvicesVM = koinViewModel(qualifier<AdvicesVM>())
    val historyVM: HistoryVM = koinViewModel(qualifier<HistoryVM>())
    val storiesVM: StoriesVM = koinViewModel(qualifier<StoriesVM>())
    val mainArticleState = mainVM.articlesSF.collectAsState().value
    NavHost(navController = controller, startDestination = navItems[0]) {
        composable(navItems[0]) { MainScreen(mainArticleState, modifier) }
        composable(navItems[1]) { ParishLife(parishLifeVM.collectSFValue(), parishLifeVM::getParishLifeArticles, modifier) }
        composable(navItems[2]) { Schedule(mainArticleState, modifier) }
        composable(navItems[3]) { ANTProgressIndicator(modifier) }
        composable(navItems[4]) { YouthClub(youthClubVM.collectSFValue(), youthClubVM::getYouthClubArticles, modifier) }
        composable(navItems[5]) { Priesthood(mainArticleState, modifier) }
        composable(navItems[6]) { Advices(advicesVM.collectSFValue(), advicesVM::getAdviceArticles, modifier) }
        composable(navItems[7]) { History(historyVM.collectSFValue(), historyVM::getHistoryArticles, modifier) }
        composable(navItems[8]) { Sacraments(mainArticleState, modifier) }
        composable(navItems[9]) { ANTProgressIndicator(modifier) }
        composable(navItems[10]) { Volunteerism(mainArticleState, modifier) }
        composable(navItems[11]) { Stories(storiesVM.collectSFValue(), storiesVM::getStoryArticles, modifier) }
    }
    if(mainArticleState.isLoading) ANTProgressIndicator(modifier)
    if(mainArticleState.hasError) LocalContext.current.displayMessage(ANTStrings.UNKNOWN_ERROR)
}
@Composable
private fun ANTProgressIndicator(modifier: Modifier) {
    Box(modifier.background(MaterialTheme.colorScheme.background)) {
        CircularProgressIndicator(Modifier.align(Alignment.Center))
    }
}
private fun Context.displayMessage(message: String) = Toast.makeText(this, message, Toast.LENGTH_SHORT).show()
@Composable
private fun ArticleVM.collectSFValue() = articlesSF.collectAsState().value