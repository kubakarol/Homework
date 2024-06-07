import { createStore } from 'vuex';
import axios from 'axios';

const store = createStore({
  state: {
    token: localStorage.getItem('token') || '',
    user: JSON.parse(localStorage.getItem('user')) || null,
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
    clearAuthData(state) {
      state.token = '';
      state.user = null;
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      delete axios.defaults.headers.common['Authorization'];
    }
  },
  actions: {
    login({ commit }, authData) {
      return axios.post('https://yourapiurl.com/api/auth/login', {
        email: authData.email,
        password: authData.password
      })
      .then(response => {
        const token = response.data.token;
        const user = response.data.user;
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
      const user = JSON.parse(localStorage.getItem('user'));
      commit('setToken', token);
      commit('setUser', user);
    },
    logout({ commit }) {
      commit('clearAuthData');
    }
  },
  getters: {
    isAuthenticated(state) {
      return !!state.token;
    },
    getUser(state) {
      return state.user;
    }
  },
  modules: {}
});

export default store;
