import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService, User } from '../auth.service';

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
        next: (user: User) => {
          console.log(user)
          this.authService.setUser(user);
          alert(`Willkommen, ${this.vorname} ${this.nachname}!`);
          
          if(this.authService.isAdmin()) {
            this.router.navigate([ '/admin-form'])
            
          } else if(this.authService.isManager()){
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
