import { Component, inject, signal } from '@angular/core';
import { NgIf } from '@angular/common';
import { User } from '../models/user';
import { UserService} from '../services/user.service';
import { ToolbarComponent } from '../toolbar/toolbar.component';

@Component({
  selector: 'app-users',
  imports: [ToolbarComponent, NgIf],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
  private userService = inject(UserService);

  user = signal<User | null>(null);

  loadUser() {
    this.userService.getCurrentUser().subscribe({
      next: (data) => this.user.set(data)
    });
  }
}
