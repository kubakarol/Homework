import { createRouter, createWebHistory } from '@ionic/vue-router';
import HomePage from '../views/HomePage.vue';
import RegisterPage from '../views/RegisterPage.vue';
import LoginPage from '../views/LoginPage.vue';
import CreatePostPage from '../views/CreatePostPage.vue';
import UsersPostsPage from '../views/UsersPostsPage.vue';
import UserProfilePage from '../views/UserProfilePage.vue';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomePage
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterPage
  },
  {
    path: '/login',
    name: 'Login',
    component: LoginPage
  },
  {
    path: '/create-post',
    name: 'CreatePost',
    component: CreatePostPage
  },
  {
    path: '/user-posts',
    name: 'UserPosts',
    component: UsersPostsPage
  },
  {
    path: '/user-profile/:userId',
    name: 'UserProfile',
    component: UserProfilePage
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;
