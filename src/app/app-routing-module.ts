import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
<<<<<<< HEAD
import { Login } from '../login';
import { UserForm } from './user-form/user-form';
import { AdminForm } from './admin-form/admin-form';

const routes: Routes = [
  {path: '', component: Login},
  {path: 'user-form', component: UserForm},
  {path: 'admin-form', component: AdminForm}
=======
import { StartSeite } from './start-seite/start-seite';
import { UserForm } from './user-form/user-form';
import { AdminForm } from './admin-form/admin-form';
import { ManagerForm } from './manager-form/manager-form';

const routes: Routes = [
  {path: "", component: StartSeite},
  {path: "user-form", component: UserForm},
  {path: "manager-form", component: ManagerForm},
  {path: "admin-form", component: AdminForm},
  {path: "**", redirectTo: ""}
>>>>>>> 1029d18d8e03322f5bdb506253564d3c1c2bb079
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
