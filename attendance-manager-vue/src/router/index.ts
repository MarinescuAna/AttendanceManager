import OrganizationView from '@/views/OrganizationView.vue'
import CreateSingleUserView from '@/views/manage-users-views/CreateSingleUserView.vue'
import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import NotFoundView from '../views/NotFoundView.vue'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    // default route
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    // route for the view that contains the departments and specializations
    path: '/organization',
    name: 'organization',
    component: OrganizationView
    //TODO guard -> admin only
  },
  {
    // route for the view that contains the form for adding a new user
    path: '/create-user',
    name: 'create-user',
    component: CreateSingleUserView
    //TODO guard -> admin only
  },
  {
    // route for the about page
    path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/AboutView.vue')
  },
  {
    // route for the login page
    path: "/login",
    name: "login",
    component: LoginView,
  },
  {
    // NotFound route
    path: '/:pathMatch(.*)',
    component: NotFoundView
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
