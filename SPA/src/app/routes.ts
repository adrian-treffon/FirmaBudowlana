import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { OrdersComponent } from './orders/orders.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'orders', component: OrdersComponent},
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'}
];
