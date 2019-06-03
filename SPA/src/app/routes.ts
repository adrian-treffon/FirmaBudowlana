import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { OrdersComponent } from './orders/orders.component';
import { NewOrderComponent } from './newOrder/newOrder.component';
import { AdminMenuComponent } from './admin-menu/admin-menu.component';
import { WorkerListComponent } from './worker-list/worker-list.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'orders', component: OrdersComponent},
            { path: 'newOrder', component: NewOrderComponent },
            { path: 'admin-menu', component: AdminMenuComponent },
            { path: 'worker-list', component: WorkerListComponent}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'}
];
