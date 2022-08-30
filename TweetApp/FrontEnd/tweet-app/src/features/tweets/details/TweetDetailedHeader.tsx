import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Button, Header, Item, Segment, Image } from "semantic-ui-react";
import { Tweet } from "../../../app/models/Tweet";
import { useStore } from "../../../app/stores/store";

interface Props {
	tweet: Tweet;
}

export default observer(function TweetDetailedHeader({ tweet }: Props) {
	const { tweetStore, userStore } = useStore();
	const { postALike } = tweetStore;
	const { user } = userStore;
	const [like, setLike] = useState(true);

	const handleLike = (id: number) => {
		postALike(id, user!);
		setLike(false);
	};

	useEffect(() => {
		if (tweetStore.userTweetLikeRegistry.size <= 1) {
			tweetStore.loadLikeUsers();
		}
	}, [tweetStore.userTweetLikeRegistry.size, tweetStore.userTweetLikeRegistry.values.length, tweetStore.loadLikeUsers, tweetStore.loading]);

	return (
		<Segment.Group>
			<Segment>
				<Item.Group>
					<Item>
						<Item.Content>
							<Header size="huge" content={`#${tweet.tag}`} />
							<p>{tweet.datePosted}</p>
							<p>
								Hosted by{" "}
								<strong>
									{tweet.user?.firstName} {tweet.user?.lastName}
								</strong>
							</p>
						</Item.Content>
					</Item>
				</Item.Group>
			</Segment>
			<Segment clearing attached="bottom">
				<Button color="teal" icon="like" onClick={() => handleLike(tweet.id)} />
			</Segment>
		</Segment.Group>
	);
});
