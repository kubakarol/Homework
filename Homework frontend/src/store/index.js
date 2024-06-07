import { createStore } from 'vuex';

const store = createStore({
  state: {
    token: localStorage.getItem('token') || ''
  },
  mutations: {
    setToken(state, token) {
      state.token = token;
      localStorage.setItem('token', token);
    },
    logout(state) {
      state.token = '';
      localStorage.removeItem('token');
    }
  },
  actions: {},
  modules: {}
});

export default store;
