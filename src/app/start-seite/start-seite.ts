import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { error } from 'console';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-start-seite',
  standalone: false,
  templateUrl: './start-seite.html',
  styleUrls: ['./start-seite.css']
})
export class StartSeite {
  vorname: string = "";
  nachname: string = "";
  
  constructor(private router: Router, private authService: AuthService){}

  onSubmit(){
    if(this.vorname && this.nachname){
      this.authService.login(this.vorname, this.nachname).subscribe({
        next: (user: any) => {
          console.log(user)
          this.authService.setUser(user);
          alert(`Willkommen, ${this.vorname} ${this.nachname}!`);
          
          if(user.roleId === 2) {
            this.router.navigate(['/manager-form'])
          } else {
            this.router.navigate(['/user-form']);
          }
        },
        error: () => alert("Benutzer nicht gefunden")
      }); 
    } 
  }
}
