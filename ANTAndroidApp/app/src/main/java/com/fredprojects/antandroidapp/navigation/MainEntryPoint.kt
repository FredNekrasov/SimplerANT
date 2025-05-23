package com.fredprojects.antandroidapp.navigation

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.itemsIndexed
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.outlined.DateRange
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.runtime.saveable.rememberSaveable
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalUriHandler
import androidx.compose.ui.unit.dp
import androidx.navigation.compose.currentBackStackEntryAsState
import androidx.navigation.compose.rememberNavController
import com.fredprojects.antandroidapp.presentation.core.*
import kotlinx.coroutines.launch

@Composable
fun MainEntryPoint() {
    val uriHandler = LocalUriHandler.current
    val navItems = ANTStrings.screens
    val scope = rememberCoroutineScope()
    val controller = rememberNavController()
    val drawerState = rememberDrawerState(DrawerValue.Closed)
    var selectedItemIndex by rememberSaveable { mutableIntStateOf(0) }
    val navigateTo: (Int, String) -> Unit = { index, route ->
        controller.navigate(route)
        selectedItemIndex = index
        scope.launch { drawerState.close() }
    }
    ModalNavigationDrawer(
        drawerContent = {
            ModalDrawerSheet {
                LazyColumn(Modifier.fillMaxSize(), verticalArrangement = Arrangement.Center) {
                    itemsIndexed(navItems) { index, route ->
                        ANTNavigationDrawerItem(
                            text = route,
                            selected = index == selectedItemIndex,
                            onClick = {
                                when(route) {
                                    ANTStrings.SPIRITUAL_TALKS -> uriHandler.openUri(ANTStrings.SPIRITUAL_TALKS_URL)
                                    ANTStrings.INFORMATION -> uriHandler.openUri(ANTStrings.INFORMATION_URL)
                                    else -> navigateTo(index, route)
                                }
                            }
                        )
                        Spacer(Modifier.height(2.dp))
                    }
                }
            }
        }, drawerState = drawerState) {
        val currentRoute = controller.currentBackStackEntryAsState().value?.destination?.route
        Scaffold(
            Modifier.fillMaxSize(),
            topBar = { ANTTopAppBar { scope.launch { drawerState.open() } } },
            floatingActionButton = { if(currentRoute != navItems[2]) ANTFloatingActionButton({ navigateTo(2, navItems[2]) }, Icons.Outlined.DateRange) },
        ) { padding ->
            ANTNavHost(controller, Modifier.fillMaxSize().padding(padding))
        }
    }
}