<template>
  <v-container>
    <v-card-title>
      <span class="text-h5 ma-4"> {{ item.name }}</span>
      <v-spacer></v-spacer>
      <!--Edit button-->
      <v-dialog v-model="dialog" max-width="50%">
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on">
            <v-icon>mdi-pencil</v-icon>
          </v-btn>
        </template>
        <ChangeDepartmentDialog :department="item" @save="dialog = false" />
      </v-dialog>
    </v-card-title>
    <v-divider></v-divider>
    <v-card-text>
      <v-row justify="center">
        <v-col>
          <h4>Specializations:</h4>
        </v-col>
        <v-col>
          <v-list-item-group v-if="specializations.length > 0">
            <v-list-item v-for="child in specializations" :key="child.id">
              <v-list-item-content>
                <v-list-item-title>{{ child.name }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-item-group>
          <p v-else>This department has no specializations defined!</p>
        </v-col>
      </v-row>
    </v-card-text>
  </v-container>
</template>
  
  <script lang="ts">
import Vue from "vue";
import { EventBus } from "@/main";
import { EVENT_BUS_RELOAD_ORGANIZATIONS } from "@/shared/constants";
import ChangeDepartmentDialog from "./ChangeDepartmentDialog.vue";
import { DepartmentModule } from "@/modules/department";
import { SpecializationModule } from "@/modules/specialization";
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  name: "DepartmentInfoCardComponent",
  components: {
    ChangeDepartmentDialog,
  },
  props: {
    /** The current department */
    item: {
      type: Object as () => DepartmentModule,
    },
  },
  data: function () {
    return {
      /** Flag for Change department dialog */
      dialog: false,
    };
  },
  computed: {
    /** Filter the specializations and display only the once related to a specific department */
    specializations: function (): SpecializationModule[] {
      return storeHelper.specializationStore.specializations.filter(
        (specialization) => specialization.departmentId == this.item.id
      );
    },
  },
  /** Load the specializations form the API */
  created: async function () {
    await storeHelper.specializationStore.loadSpecializations();
  },
  /** Emit an event using EventBus in order to update the treeview whenever the user add a new department or specialization */
  mounted: function () {
    EventBus.$on(EVENT_BUS_RELOAD_ORGANIZATIONS, () => {
      EventBus.$off(EVENT_BUS_RELOAD_ORGANIZATIONS);
    });
  },
});
</script>