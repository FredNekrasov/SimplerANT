plugins {
    alias(libs.plugins.android.application)
    alias(libs.plugins.kotlin.android)
    alias(libs.plugins.kotlin.compose)
    alias(libs.plugins.kotlin.serialization)
    alias(libs.plugins.sqldelight)
}

android {
    namespace = "com.fredprojects.antandroidapp"
    compileSdk = 35

    defaultConfig {
        applicationId = "com.fredprojects.antandroidapp"
        minSdk = 26
        targetSdk = 35
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(getDefaultProguardFile("proguard-android-optimize.txt"), "proguard-rules.pro")
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }
    kotlinOptions {
        jvmTarget = "17"
    }
    buildFeatures {
        compose = true
    }
}
sqldelight {
    databases {
        create("ANTDatabase") {
            packageName = "com.fredprojects.antandroidapp.data"
        }
    }
}
composeCompiler {
    reportsDestination = layout.buildDirectory.dir("composeCompiler")
    metricsDestination = layout.buildDirectory.dir("composeMetrics")
}
dependencies {
    implementation(libs.bundles.koin)
    implementation(libs.bundles.network) // Ktor and kotlinx serialization
    implementation(libs.bundles.database)

    implementation(platform(libs.androidx.compose.bom))
    implementation(libs.bundles.app)
    testImplementation(libs.junit)
    androidTestImplementation(platform(libs.androidx.compose.bom))
    androidTestImplementation(libs.bundles.android.test)
    debugImplementation(libs.androidx.ui.tooling)
    debugImplementation(libs.androidx.ui.test.manifest)
}