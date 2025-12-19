import { NgModule, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { BrowserModule, provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { ReactiveFormsModule } from '@angular/forms';
import { UserForm } from './user-form/user-form';
import { AdminForm } from './admin-form/admin-form';
<<<<<<< HEAD
import { Login } from '../login';
=======
import { StartSeite } from './start-seite/start-seite';
import { RouterModule } from '@angular/router';
import { ManagerForm } from './manager-form/manager-form';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
>>>>>>> 1029d18d8e03322f5bdb506253564d3c1c2bb079

@NgModule({
  declarations: [
    App,
<<<<<<< HEAD
    Login,
    UserForm,
    AdminForm,
=======
    StartSeite,
    UserForm,
    ManagerForm,
    AdminForm
>>>>>>> 1029d18d8e03322f5bdb506253564d3c1c2bb079
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
<<<<<<< HEAD
    FormsModule,
    ReactiveFormsModule
=======
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    CommonModule
>>>>>>> 1029d18d8e03322f5bdb506253564d3c1c2bb079
    
  ],
  providers: [
    provideClientHydration(withEventReplay())
  ],
  bootstrap: [App]
})
export class AppModule { }
