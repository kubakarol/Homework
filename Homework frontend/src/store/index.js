import { createStore } from 'vuex';
import axios from 'axios';
import jwt_decode from 'jwt-decode';

const store = createStore({
  state: {
    token: localStorage.getItem('token') || '',
    user: JSON.parse(localStorage.getItem('user')) || null,
    posts: [] // Add posts state
  },
  mutations: {
    setToken(state, token) {
      state.token = token;
      localStorage.setItem('token', token);
      axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    },
    setUser(state, user) {
      state.user = user;
      localStorage.setItem('user', JSON.stringify(user));
    },
    setPosts(state, posts) { // Add mutation to set posts
      state.posts = posts;
    },
    clearAuthData(state) {
      state.token = '';
      state.user = null;
      state.posts = [];
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      delete axios.defaults.headers.common['Authorization'];
    }
  },
  actions: {
    login({ commit }, authData) {
      return axios.post('https://localhost:7195/api/auth/login', {
        email: authData.email,
        password: authData.password
      })
      .then(response => {
        const token = response.data.token;
        const decodedToken = jwt_decode(token);
        const user = {
          id: decodedToken.nameid, // Assuming the UserId is stored in the 'nameid' claim
          userName: decodedToken.unique_name // Adjust based on your token claims
        };
        commit('setToken', token);
        commit('setUser', user);
      })
      .catch(error => {
        console.error('Error during login:', error);
        throw error;
      });
    },
    autoLogin({ commit }) {
      const token = localStorage.getItem('token');
      if (!token) {
        return;
      }
      const decodedToken = jwt_decode(token);
      const user = {
        id: decodedToken.nameid, // Assuming the UserId is stored in the 'nameid' claim
        userName: decodedToken.unique_name // Adjust based on your token claims
      };
      commit('setToken', token);
      commit('setUser', user);
    },
    logout({ commit }) {
      commit('clearAuthData');
    },
    fetchPosts({ commit }) { // Add action to fetch posts
      return axios.get('https://localhost:7195/api/Post/getAll')
        .then(response => {
          commit('setPosts', response.data);
        })
        .catch(error => {
          console.error('Error fetching posts:', error);
        });
    }
  },
  getters: {
    isAuthenticated(state) {
      return !!state.token;
    },
    getUser(state) {
      return state.user;
    },
    getPosts(state) { // Add getter for posts
      return state.posts;
    }
  },
  modules: {}
});

export default store;
