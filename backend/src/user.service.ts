import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

export interface User {
    IdUser: number;
    Vorname: string;
    Nachname: string;
    IdRolle: number;
}

@Injectable ({providedIn: "root"})
export class UserService{
    private apiUrl = "http://localhost:5156/api/user";

    constructor (private http: HttpClient) {}

    getUserByName(Vorname: string, Nachname: string): Observable<User | null>
    {
        return this.http.get<User | null>(`${this.apiUrl}/byname?vorname=${Vorname}&nachname=${Nachname}`);
    }
}