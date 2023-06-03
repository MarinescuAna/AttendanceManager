<template>
  <v-container>
    <v-card-title>
      <span class="text-h5 ma-4"> {{ department.name }}</span>
      <v-spacer></v-spacer>
      <!--Edit button-->
      <v-dialog v-model="dialog" max-width="50%" :fullscreen="isMobile">
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on">
            <v-icon>mdi-pencil</v-icon>
          </v-btn>
        </template>
        <ChangeDepartmentDialog
          class="pa-1"
          :department="department"
          @save="dialog = false"
          @close="dialog = false"
        />
      </v-dialog>
    </v-card-title>
    <v-divider></v-divider>
    <v-card-text>
      <v-row justify="center" v-if="specializations.length > 0">
        <v-col>
          <h4>Specializations:</h4>
        </v-col>
        <v-col>
          <v-list-item-group>
            <v-list-item v-for="child in specializations" :key="child.id">
              <v-list-item-content>
                <v-list-item-title>{{ child.name }}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list-item-group>
        </v-col>
      </v-row>
      <MessageComponent
        description="This department has no specialization defined!"
        fontWeight="bold"
        icon="mdi-information"
        :color="WARNING_AMBER_DARKEN_4"
        v-else
      />
    </v-card-text>
  </v-container>
</template>
  
  <script lang="ts">
import Vue from "vue";
import { EventBus } from "@/main";
import { EVENT_BUS_RELOAD_ORGANIZATIONS } from "@/shared/constants";
import ChangeDepartmentDialog from "./ChangeDepartmentDialog.vue";
import { DepartmentViewModule } from "@/modules/department";
import { SpecializationViewModule } from "@/modules/specialization";
import storeHelper from "@/store/store-helper";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import MessageComponent from "../shared-components/MessageComponent.vue";

export default Vue.extend({
  name: "DepartmentDetailsExpanded",
  components: {
    ChangeDepartmentDialog,
    MessageComponent,
  },
  props: {
    department: {
      type: Object as () => DepartmentViewModule,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      dialog: false,
    };
  },
  computed: {
    /** Filter the specializations and display only the once related to a specific department */
    specializations: function (): SpecializationViewModule[] {
      return storeHelper.specializationStore.specializations.filter(
        (specialization) => specialization.departmentId == this.department.id
      );
    },
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.xs;
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