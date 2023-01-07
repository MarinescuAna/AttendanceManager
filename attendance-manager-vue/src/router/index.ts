import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    redirect: '/home'
  },
  {
    // default route
    path: '/home',
    component: () => import('../views/HomeView.vue')
  },
  {
    // Route for the list of users
    path: '/users',
    component: () => import('../views/manage-users-views/UsersView.vue')
    // TODO guard -> admin only
  },
  {
    // route for the view that contains the departments and specializations
    path: '/organization',
    component: () => import('../views/OrganizationView.vue'),
    children:[
      { path: 'new-department',name:'new-department', component: () => import('../components/department-specialization/CreateDepartmentComponent.vue')},
      { path: 'new-specialization',name:'new-specialization', component: () => import('../components/department-specialization/CreateSpecializationComponent.vue')}
    ]
    //TODO guard -> admin only
  },
  {
    // route for the view that contains the form for adding a new user
    path: '/create-user',
    component: () => import('../views/manage-users-views/CreateSingleUserView.vue')
    //TODO guard -> admin only
  },
  {
    // route for the about page
    path: '/about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import('../views/AboutView.vue')
  },
  {
    // route for the login page
    path: "/login",
    name: "login",
    component: () => import('../views/LoginView.vue'),
  },
  {
    // NotFound route
    // WARNING: DO NOT add any new route after this route, because it will never be matched!!!
    path: '/:pathMatch(.*)',
    component: () => import('../views/NotFoundView.vue')
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes,
  //set those two below properties in order to highlight the selected route
  linkActiveClass: "active",
  linkExactActiveClass: "active"
})

export default router
