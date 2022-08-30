import { createContext, useContext } from "react";
import CommonStore from "./commonStore";
import ModalStore from "./modalStore";
import TweetStore from "./tweetStore";
import UserStore from "./userStore";

interface Store {
	userStore: UserStore;
	commonStore: CommonStore;
	modalStore: ModalStore;
	tweetStore: TweetStore;
}

export const store: Store = {
	userStore: new UserStore(),
	commonStore: new CommonStore(),
	modalStore: new ModalStore(),
	tweetStore: new TweetStore(),
};

export const StoreContext = createContext(store);

export function useStore() {
	return useContext(StoreContext);
}
