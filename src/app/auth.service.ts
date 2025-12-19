import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

export interface User {
    idUser: number;
    vorname: string;
    nachname: string;
    roleId: number;
    rolle?: string;
}

@Injectable({providedIn: 'root'})
export class AuthService{
    currentUser: User | null = null;

    constructor(private http: HttpClient) {}

    login(vorname: string, nachname: string) {
        return this.http.post<User>('/api/auth/login', {vorname, nachname});
    }

    setUser(user: User) {
        this.currentUser = user;
        localStorage.setItem('user', JSON.stringify(user));
    }
    
    getUser() {
        if(!this.currentUser){
            this.currentUser = JSON.parse(localStorage.getItem('user') || 'null');
        }
        return this.currentUser;
    }

    isAdmin(): boolean{
        return this.getUser()?.roleId === 1;
    }

    isManager(): boolean{
        return this.getUser()?.roleId === 2;
    }

    isUser(): boolean{
        return this.getUser()?.roleId === 3;
    }

    logout(): void{
        this.currentUser = null;
        localStorage.removeItem('user');
    }
}