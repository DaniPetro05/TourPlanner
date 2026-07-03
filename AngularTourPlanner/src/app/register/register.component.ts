import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  imports: [FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  username = '';
  email = '';
  password = '';
  
  constructor(private auth: AuthService, private router: Router) {}

  register() {
    this.auth.register(
      this.username,
      this.email,
      this.password
    ).subscribe({
      next: () => {
        alert("Registration successful.");
        this.router.navigate(['/login']);
      },
      error: err => {
        alert(err.error);
      }
    });
  }
}
