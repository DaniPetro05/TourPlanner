import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ToolbarComponent } from './toolbar/toolbar.component';
import { UsersComponent } from './users/users.component';
import { HomeComponent } from './home/home.component';
import { ToursComponent } from './tours/tours.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { authGuard } from './guards/auth.guard';
import { guestGuard } from './guards/guest.guard';

export const routes: Routes = [
    //{ path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent, canActivate: [guestGuard] },
    { path: 'register', component: RegisterComponent, canActivate: [guestGuard]  },
    { path: 'user', component: UsersComponent, canActivate: [authGuard] },
    { path: 'tours', component: ToursComponent, canActivate: [authGuard]  },
    { path: 'home', component: HomeComponent },
    //{ path: '**', redirectTo: 'home' }
    { path: '**', redirectTo: 'login' }
];
