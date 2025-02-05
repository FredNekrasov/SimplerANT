package com.fredprojects.antandroidapp.presentation.core

import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.snapshots.SnapshotStateList

object ANTStrings {
    const val MAIN_TITLE = "Храм Александра Невского"
    const val UNKNOWN_ERROR = "Неизвестная ошибка"
    // screen routes
    const val MAIN = "Главная страница"
    private const val PARISH_LIFE = "Приходская жизнь"
    const val SCHEDULE = "Расписание Богослужений"
    const val SPIRITUAL_TALKS = "Духовные беседы"
    private const val YOUTH_CLUB = "Молодежный клуб"
    const val PRIESTHOOD = "Священство"
    private const val ADVICES = "Советы священника"
    private const val HISTORY = "История"
    private const val SACRAMENTS_AND_CONTACTS = "Требы и контакты"
    const val INFORMATION = "Информация"
    const val VOLUNTEERISM = "Приходская добровольческая служба"
    private const val STORIES = "Рассказы"
    // URLs
    const val SPIRITUAL_TALKS_URL = "https://hramalnevskogo.ru/page40967215.html"
    const val INFORMATION_URL = "https://hramalnevskogo.ru/page42533272.html"
    // other
    const val TELEGRAM = "Телеграм"
    const val VK = "VK"
    const val SACRAMENTS = "Требы"
    const val CONTACTS = "Контакты"

    val screens: SnapshotStateList<String> = mutableStateListOf(MAIN, PARISH_LIFE, SCHEDULE, SPIRITUAL_TALKS, YOUTH_CLUB, PRIESTHOOD, ADVICES, HISTORY, SACRAMENTS_AND_CONTACTS, INFORMATION, VOLUNTEERISM, STORIES)
}