import { NgModule, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { BrowserModule, provideClientHydration, withEventReplay } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { ReactiveFormsModule } from '@angular/forms';
import { UserForm } from './user-form/user-form';
import { AdminForm } from './admin-form/admin-form';
import { Login } from '../login';

@NgModule({
  declarations: [
    App,
    Login,
    UserForm,
    AdminForm,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule
    
  ],
  providers: [
    provideClientHydration(withEventReplay())
  ],
  bootstrap: [App]
})
export class AppModule { }
