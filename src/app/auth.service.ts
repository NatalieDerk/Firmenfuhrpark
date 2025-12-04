import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})
export class AuthService{
    currentUser: any = null;

    constructor(private http: HttpClient) {}

    login(vorname: string, nachname: string) {
        return this.http.post('/api/auth/login', {vorname, nachname});
    }

    setUser(user: any) {
        this.currentUser = user;
        localStorage.setItem('user', JSON.stringify(user));
    }
    
    getUser() {
        if(!this.currentUser){
            this.currentUser = JSON.parse(localStorage.getItem('user') || 'null');
        }
        return this.currentUser;
    }

    isManager(){
        return this.getUser()?.RoleId === 2;
    }
}