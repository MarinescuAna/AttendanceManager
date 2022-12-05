import Vue from 'vue'
import Vuex from 'vuex'
import organization, { namespace as organizationNamespace} from "./modules/organization";
import user, { namespace as userNamespace} from "./modules/user";
import createPersistedState from "vuex-persistedstate";

//Load Vuex
Vue.use(Vuex)

const modules: { [id: string]: any } = {};

modules[organizationNamespace] = organization;
modules[userNamespace] = user;

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
