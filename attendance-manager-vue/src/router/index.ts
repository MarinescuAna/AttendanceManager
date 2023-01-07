import AuthService from '@/services/auth.service'
import { TokenData } from "@/shared/modules";
import { Role } from '@/shared/enums'
import Vue from 'vue'
import VueRouter, { RouteConfig } from 'vue-router'

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    redirect: "{name:'home'}"
  },
  {
    // default route
    path: '/home',
    name: 'home',
    component: () => import('../views/HomeView.vue'),
    meta: {
      title: 'Home',
      requireAuth: false,
      role: Role.All
    },
  },
  {
    // Route for the list of users
    path: '/users',
    name: 'users',
    component: () => import('../views/manage-users-views/UsersView.vue'),
    meta: {
      title: 'Users',
      requireAuth: true,
      role: Role.Admin
    },
    // TODO guard -> admin only
  },
  {
    // route for the view that contains the departments and specializations
    path: '/organization',
    name: 'organization',
    component: () => import('../views/departments-specializations/OrganizationView.vue'),
    children: [
      {
        meta: {
          onBack: () => { router.push({ name: 'organization' }) },
          title: 'Create Department',
          requireAuth: true,
          role: Role.Admin
        },
        path: 'new-department',
        name: 'new-department',
        component: () => import('../views/departments-specializations/CreateDepartmentView.vue')
      },
      {
        meta: {
          onBack: () => { router.push({ name: 'organization' }) },
          title: 'Create Specialization',
          requireAuth: true,
          role: Role.Admin
        },
        path: 'new-specialization',
        name: 'new-specialization',
        component: () => import('../views/departments-specializations/CreateSpecializationView.vue')
      }
    ],
    meta: {
      title: 'Organization',
      requireAuth: true,
      role: Role.Admin
    },
    //TODO guard -> admin only
  },
  {
    // route for the view that contains the form for adding a new user
    path: '/create-user',
    name: 'create-user',
    component: () => import('../views/manage-users-views/CreateSingleUserView.vue'),
    meta: {
      title: 'Create User',
      requireAuth: true,
      role: Role.Admin
    },
    //TODO guard -> admin only
  },
  {
    // route for the about page
    path: '/about',
    name: 'about',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import('../views/AboutView.vue'),
    meta: {
      title: 'About',
      requireAuth: false,
      role: Role.All
    },
  },
  {
    // route for the login page
    path: "/login",
    name: "login",
    component: () => import('../views/LoginView.vue'),
    meta: {
      title: 'Login',
      requireAuth: false,
      role: Role.All
    },
  },
  {

    path: '/unauthorized',
    name: 'unauthorized',
    component: () => import('../views/routing-views/UnauthorizedView.vue'),
    meta: {
      title: 'Unauthorized',
      requireAuth: false,
      role: Role.All
    },
  },
  {
    // NotFound route
    // WARNING: DO NOT add any new route after this route, because it will never be matched!!!
    path: '/:pathMatch(.*)',
    component: () => import('../views/routing-views/NotFoundView.vue'),
    meta: {
      title: 'Page Not Found',
      requireAuth: false,
      role: Role.All
    },
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

router.beforeEach(async (to, from, next) => {
  // Redirect user to login page each time when he try to access a page that needs authentication
  if (to.meta?.requireAuth) {
    const tokenData = AuthService.getDataFromToken();

    if (tokenData && tokenData == null) {
      next({ name: 'login' });
    } else {
      if (Role[to.meta?.role] != (tokenData as TokenData).role.toString()) {
        next({ name: 'unauthorized' });
      } else {
        next();
      }
    }
  } else {
    next();
  }
});

router.afterEach((to) => {
  if (to.meta?.title) {
    document.title = to.meta?.title;
  } else {
    document.title = "AttendanceManager";
  }
});

export default router
