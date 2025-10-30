import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Login } from '../login';
import { UserForm } from './user-form/user-form';
import { AdminForm } from './admin-form/admin-form';

const routes: Routes = [
  {path: '', component: Login},
  {path: 'user-form', component: UserForm},
  {path: 'admin-form', component: AdminForm}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
