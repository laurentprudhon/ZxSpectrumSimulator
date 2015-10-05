# gmake

NAME = zexall2
VERSION = 0.1
PKG := $(NAME)-$(VERSION)

PROGS := zexall2 zexccfb

all: $(addsuffix .tap,$(PROGS))

%.out : %.asm
	sjasmplus --nologo $<

%.tap : loader.bas %.out
	mktap -b $(basename $(word 1,$^)) 10 <$(word 1,$^) >$@
	mktap    $(basename $(word 2,$^)) 32768 <$(word 2,$^) >>$@

FILES := Makefile $(addsuffix .asm,$(PROGS)) loader.bas $(addsuffix .tap,$(PROGS))

dist:
	ln -s . $(PKG)
	tar czf $(PKG).tar.gz --group=root --owner=root $(addprefix $(PKG)/, $(FILES))
	zip $(PKG).zip $(addprefix $(PKG)/, $(FILES))
	rm $(PKG)

clean:
	rm -rf *.out *.tap

tidy: clean
	rm -rf $(PROGS)
