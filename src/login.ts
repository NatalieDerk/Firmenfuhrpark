import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { UserService } from "./user.service";
import { throwError } from "rxjs";

@Component({
    selector: "app-login",
    template: 
    `<div class="min-h-screen flex items-center justify-center">
        <form (ngSubmit)="login()" #loginForm="ngForm" class="border-2 border-blue-900 rounded-md p-8">
            <h1 class="text-3xl mb-4">Fuhrpark Login</h1>
            <div class="mb-4">
                <label class="block mb-1">Vorname:</label>
                <input type="text" name="Vorname" [(ngModel)]="Vorname" required class="border border-blue-900 rounded-md p-1 w-full"/>
            </div>
            <div class="mb-4">
                <label class="block mb-1">Nachname:</label>
                <input type="text" name="Nachname" [(ngModel)]="Nachname" required class="border border-blue-900 rounded-md p-1 w-full"/>
            </div>
            <button type="submit" [disabled]="!loginForm.form.valid" class="bg-blue-900 text-white p-2 rounded-md w-full">Login</button>
            <div *ngIf="error" class="text-red-600 mt-2">{{error}}</div>
        </form>
    </div>`
})
export class Login{
    Vorname = "";
    Nachname = "";
    error = "";

    constructor (private useService: UserService, private router: Router) {}

    login(){
        this.useService.getUserByName(this.Vorname, this.Nachname).subscribe({
            next: user => {
                if (user) {
                    console.log("Eingeloggt als:", user);
                    this.router.navigate(["/user-form"]);
                } else {
                    this.error = "Benutzer nicht gefunden!";
                }
            },
            error: () => this.error = "Fehler beim Abrufen des Benutzers"
        })
    }
    
}