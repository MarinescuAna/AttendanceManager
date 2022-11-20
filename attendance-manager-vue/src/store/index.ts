import Vue from 'vue'
import Vuex from 'vuex'
import auth, { namespace as authNamespace } from "./modules/auth";
import createPersistedState from "vuex-persistedstate";

//Load Vuex
Vue.use(Vuex)

const modules: { [id: string]: any } = {};

modules[authNamespace] = auth;

//Create store
export default new Vuex.Store({
  getters: {
  },
  mutations: {
  },
  actions: {
  },
  modules: modules,
  plugins: [createPersistedState()]
})
