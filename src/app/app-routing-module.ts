import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
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
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
